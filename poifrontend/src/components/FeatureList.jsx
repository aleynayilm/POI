import React, { useEffect, useState } from "react";
import axios from "axios";

const FeatureList = () => {
    const [features, setFeatures] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        axios
            .get("https://localhost:7020/MapObject/GetAll")
            .then((res) => {
                setFeatures(res.data.data);
                setLoading(false);
            })
            .catch((err) => {
                console.error("Veri alınamadı", err);
                setLoading(false);
            });
    }, []);

    if (loading) return <p>Yükleniyor...</p>;

    return (
        <div style={{ marginTop: "20px" }}>
            <h2>Veritabanındaki Geometriler</h2>
            <table style={{ borderCollapse: "collapse", width: "100%" }}>
                <thead>
                    <tr style={{ backgroundColor: "#f2f2f2" }}>
                        <th style={cellStyle}>ID</th>
                        <th style={cellStyle}>İsim</th>
                        <th style={cellStyle}>WKT</th>
                        <th style={cellStyle}>İşlemler</th>
                    </tr>
                </thead>
                <tbody>
                    {features.map((feature) => (
                        <tr key={feature.id}>
                            <td style={cellStyle}>{feature.id}</td>
                            <td style={cellStyle}>{feature.name}</td>
                            <td style={cellStyle}>{feature.wkt}</td>
                            <td style={cellStyle}>
                                <button onClick={() => handleDelete(feature.id)}>Sil</button>
                                <button onClick={() => handleUpdate(feature)}>Güncelle</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

const cellStyle = {
    border: "1px solid #ddd",
    padding: "8px",
    textAlign: "left",
};

const handleDelete = (id) => {
    if (!window.confirm("Bu geometri silinsin mi?")) return;

    axios.delete(`https://localhost:7020/MapObject/Delete/${id}`)
        .then(() => {
            alert("Silme başarılı.");
            setFeatures((prev) => prev.filter((f) => f.id !== id));
        })
        .catch((err) => {
            console.error("Silme hatası", err);
        });
};

const handleUpdate = (feature) => {
    const newName = prompt("Yeni ismi girin:", feature.name);
    if (!newName) return;

    const updatedFeature = {
        ...feature,
        name: newName,
    };

    axios.put(`https://localhost:7020/MapObject/Update/${updatedFeature.id}`, updatedFeature)
        .then(() => {
            alert("Güncelleme başarılı.");
            setFeatures((prev) =>
                prev.map((f) => (f.id === feature.id ? updatedFeature : f))
            );
        })
        .catch((err) => {
            console.error("Güncelleme hatası", err);
        });
};

export default FeatureList;