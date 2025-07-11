import React, { useEffect, useRef } from "react";
import Map from "ol/Map";
import View from "ol/View";
import TileLayer from "ol/layer/Tile";
import VectorLayer from "ol/layer/Vector";
import VectorSource from "ol/source/Vector";
import OSM from "ol/source/OSM";
import Draw from "ol/interaction/Draw";
import { fromLonLat } from "ol/proj";
import WKT from "ol/format/WKT";
import Style from "ol/style/Style";
import Stroke from "ol/style/Stroke";
import Fill from "ol/style/Fill";
import CircleStyle from "ol/style/Circle";
import Overlay from "ol/Overlay";
import axios from "axios";

const AccessibleMap = ({ geometryType, onRefresh, refresh, onResetGeometryType }) => {
    const isDrawingRef = useRef(false);
    const mapRef = useRef();
    const popupRef = useRef();
    const overlayRef = useRef();
    const vectorSourceRef = useRef(new VectorSource());
    const drawRef = useRef(null);
    const mapObjectRef = useRef(null);

    const getFeatureStyle = (feature) => {
        const geometryType = feature.getGeometry().getType();
        switch (geometryType) {
            case "Point":
                return new Style({
                    image: new CircleStyle({
                        radius: 7,
                        fill: new Fill({ color: "#d81b60" }),
                        stroke: new Stroke({ color: "white", width: 2 }),
                    }),
                });
            case "Polygon":
                return new Style({
                    stroke: new Stroke({ color: "#f48fb1", width: 2 }),
                    fill: new Fill({ color: "rgba(248, 200, 220, 0.4)" }),
                });
            case "LineString":
                return new Style({
                    stroke: new Stroke({ color: "#d81b60", width: 3 }),
                });
            default:
                return null;
        }
    };
    useEffect(() => {
        const rasterLayer = new TileLayer({ source: new OSM() });
        const vectorLayer = new VectorLayer({
            source: vectorSourceRef.current,
            style: getFeatureStyle,
        });

        const overlay = new Overlay({
            element: popupRef.current,
            positioning: "bottom-center",
            stopEvent: false,
            offset: [0, -15],
        });
        overlayRef.current = overlay;

        const map = new Map({
            target: mapRef.current,
            layers: [rasterLayer, vectorLayer],
            view: new View({
                center: fromLonLat([35, 39]),
                zoom: 6,
            }),
            overlays: [overlay],
        });

        mapObjectRef.current = map;

        map.on("singleclick", function (evt) {
            if (isDrawingRef.current) return;
            const feature = map.forEachFeatureAtPixel(evt.pixel, function (feat) {
                return feat;
            });

            if (feature) {
                const name = feature.get("name");
                const id = feature.get("id");
                const format = new WKT();
                const wkt = format.writeFeature(feature);

                const content = `
                    <strong>${name}</strong><br/>
                    <small>${wkt}</small><br/><br/>
                    <button id="update-btn">Güncelle</button>
                    <button id="delete-btn">Sil</button>
                `;

                popupRef.current.innerHTML = content;
                overlay.setPosition(evt.coordinate);
                popupRef.current.style.display = "block";

                setTimeout(() => {
                    const updateBtn = document.getElementById("update-btn");
                    const deleteBtn = document.getElementById("delete-btn");

                    if (updateBtn) {
                        updateBtn.onclick = () => {
                            const newName = prompt("Yeni ismi girin:", name);
                            if (!newName) return;

                            const updatedFeature = {
                                id,
                                name: newName,
                                wkt,
                            };

                            axios.put(`https://localhost:7020/MapObject/Update/${id}`, {
                                name: newName,
                                wkt: wkt
                            })
                                .then(() => {
                                    feature.set("name", newName);
                                    alert("Güncelleme başarılı.");
                                    if (onRefresh) onRefresh();
                                    popupRef.current.style.display = "none";
                                })
                                .catch((err) => {
                                    console.error("Güncelleme hatası:", err);
                                    alert("Güncelleme hatası: " + err.message);
                                });
                        };
                    }

                    if (deleteBtn) {
                        deleteBtn.onclick = () => {
                            const confirmed = window.confirm("Silmek istediğine emin misin?");
                            if (!confirmed) return;

                            axios.delete(`https://localhost:7020/MapObject/Delete/${id}`)
                                .then(() => {
                                    alert("Silme başarılı.");
                                    if (onRefresh) onRefresh();
                                    popupRef.current.style.display = "none";
                                })
                                .catch((err) => {
                                    alert("Silme hatası: " + err.message);
                                });
                        };
                    }
                }, 0);
            } else {
                popupRef.current.style.display = "none";
            }
        });

        return () => map.setTarget(null);
    }, []);

    useEffect(() => {
        const format = new WKT();
        axios.get("https://localhost:7020/MapObject/GetAll")
            .then((res) => {
                const features = res.data.data.map(item => {
                    const feature = format.readFeature(item.wkt);
                    feature.set("name", item.name);
                    feature.set("id", item.id);
                    return feature;
                });

                vectorSourceRef.current.clear();
                vectorSourceRef.current.addFeatures(features);
            })
            .catch((err) => {
                console.error("Geometriler alınamadı", err);
            });
    }, [refresh]);

    useEffect(() => {
        if (!mapObjectRef.current) return;

        if (drawRef.current) {
            mapObjectRef.current.removeInteraction(drawRef.current);
        }

        if (!geometryType) return;

        const draw = new Draw({
            source: vectorSourceRef.current,
            type: geometryType,
        });

        draw.on("drawstart", () => {
            isDrawingRef.current = true;
            if (popupRef.current) {
                popupRef.current.style.display = "none";
            }
        });

        draw.on("drawend", (event) => {
            isDrawingRef.current = false
            const format = new WKT();
            const wkt = format.writeFeature(event.feature);

            const name = prompt("Lütfen geometriye bir isim girin:");
            if (!name) return;

            event.feature.set("name", name);
            const data = { Name: name, WKT: wkt };

            axios.post("https://localhost:7020/MapObject/Add", data)
                .then(() => {
                    alert("Geometri başarıyla eklendi!");
                    // if (onRefresh) 
                    onRefresh();
                    onResetGeometryType();
                })
                .catch((err) => {
                    console.error("Ekleme hatası:", err);
                    alert("Hata oluştu: " + err.message);
                });
        });

        mapObjectRef.current.addInteraction(draw);
        drawRef.current = draw;
    }, [geometryType]);

    useEffect(() => {
        const handleKeyDown = (e) => {
            if (e.key === "Escape" && drawRef.current) {
                drawRef.current.abortDrawing();
                console.log("Çizim ESC ile iptal edildi.");
            }
        };

        window.addEventListener("keydown", handleKeyDown);
        return () => window.removeEventListener("keydown", handleKeyDown);
    }, []);

    return (
        <div style={{ position: "relative" }}>
            <div
                ref={mapRef}
                style={{
                    width: "100%",
                    height: "500px",
                    marginTop: "10px",
                    border: "1px solid #ccc",
                }}
            />
            <div
                ref={popupRef}
                className="ol-popup"
                style={{
                    backgroundColor: "white",
                    padding: "8px",
                    border: "1px solid black",
                    borderRadius: "4px",
                    display: "none",
                    position: "absolute",
                    zIndex: 1000,
                    maxWidth: "300px",
                    whiteSpace: "normal",
                    wordWrap: "break-word",
                    boxShadow: "0 2px 6px rgba(0,0,0,0.3)",
                    fontSize: "14px",
                }}
            />
        </div>
    );
};

export default AccessibleMap;
