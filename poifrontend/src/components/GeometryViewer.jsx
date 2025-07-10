import React, { useEffect, useRef } from "react";
import Map from "ol/Map";
import View from "ol/View";
import TileLayer from "ol/layer/Tile";
import VectorLayer from "ol/layer/Vector";
import VectorSource from "ol/source/Vector";
import OSM from "ol/source/OSM";
import WKT from "ol/format/WKT";
import axios from "axios";
import { fromLonLat } from "ol/proj";

const GeometryViewer = ({ refresh }) => {
    const mapRef = useRef();
    const vectorSourceRef = useRef(new VectorSource());
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

        const format = new WKT();

        axios.get("https://localhost:7020/MapObject/GetAll")
            .then((res) => {
                const features = res.data.data.map(item =>
                    format.readFeature(item.wkt)
                );

                vectorSourceRef.current.clear();
                vectorSourceRef.current.addFeatures(features);
            })
            .catch((err) => {
                console.error("Geometriler alınamadı", err);
            });
    }, [refresh]);

    return (
        <div style={{ width: "800px", height: "500px", marginTop: "20px" }} ref={mapRef}></div>
    );
};

export default GeometryViewer;
