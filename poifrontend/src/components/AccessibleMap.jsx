import React, { useEffect, useRef } from "react";
import Map from "ol/Map";
import View from "ol/View";
import TileLayer from "ol/layer/Tile";
import VectorLayer from "ol/layer/Vector";
import OSM from "ol/source/OSM";
import VectorSource from "ol/source/Vector";
import Draw from "ol/interaction/Draw";
import { fromLonLat } from "ol/proj";
import WKT from "ol/format/WKT";
import axios from "axios";

const AccessibleMap = ({ geometryType }) => {
    const mapRef = useRef();
    const vectorSourceRef = useRef(new VectorSource());
    const drawRef = useRef(null);
    const mapObjectRef = useRef(null);

    useEffect(() => {
        const rasterLayer = new TileLayer({
            source: new OSM(),
        });

        const vectorLayer = new VectorLayer({
            source: vectorSourceRef.current,
        });

        const map = new Map({
            target: mapRef.current,
            layers: [rasterLayer, vectorLayer],
            view: new View({
                center: fromLonLat([35, 39]),
                zoom: 6,
            }),
        });

        mapObjectRef.current = map;

        return () => map.setTarget(null);
    }, []);

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

        draw.on("drawend", (event) => {
            const format = new WKT();
            const wkt = format.writeFeature(event.feature);

            const name = prompt("Lütfen geometriye bir isim girin:");
            if (!name) return;

            const data = {
                Name: name,
                WKT: wkt,
            };

            axios
                .post(`https://localhost:7020/MapObject/Add`, data)
                .then(() => {
                    alert("Geometri başarıyla eklendi!");
                })
                .catch((err) => {
                    console.error("Ekleme hatası:", err);
                    if (err.response) {
                        console.log("Hata yanıtı:", err.response);
                        alert("Hata: " + (err.response.data?.message || "Sunucudan hata döndü."));
                    } else if (err.request) {
                        console.log("İstek gönderildi ama yanıt alınamadı:", err.request);
                        alert("İstek gönderildi ama sunucudan yanıt alınamadı.");
                    } else {
                        console.log("İstek ayarlanırken hata oluştu:", err.message);
                        alert("İstek hazırlık hatası: " + err.message);
                    }
                });
        });

        mapObjectRef.current.addInteraction(draw);
        drawRef.current = draw;
    }, [geometryType]);

    return (
        <div
            ref={mapRef}
            style={{ width: "800px", height: "500px", marginTop: "10px" }}
        />
    );
};

export default AccessibleMap;
