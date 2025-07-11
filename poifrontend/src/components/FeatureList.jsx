import React, { useEffect, useState } from "react";
import axios from "axios";

const FeatureList = ({ refresh, onRefresh }) => {
    const [features, setFeatures] = useState([]);
    const [loading, setLoading] = useState(true);

    const [currentPage, setCurrentPage] = useState(1);
    const itemsPerPage = 5;

    useEffect(() => {
        setLoading(true);
        axios
            .get("https://localhost:7020/MapObject/GetAll")
            .then((res) => {
                setFeatures(res.data.data);
                setLoading(false);
                setCurrentPage(1);
            })
            .catch((err) => {
                console.error("Veri alınamadı", err);
                setLoading(false);
            });
    }, [refresh]);

    useEffect(() => {
        setLoading(true);
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
    }, [refresh]);

    const handleDelete = (id) => {
        if (!window.confirm("Bu geometri silinsin mi?")) return;

        axios.delete(`https://localhost:7020/MapObject/Delete/${id}`)
            .then(() => {
                alert("Silme başarılı.");
                if (onRefresh) onRefresh();
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
                if (onRefresh) onRefresh();
            })
            .catch((err) => {
                console.error("Güncelleme hatası", err);
            });
    };

    const indexOfLastItem = currentPage * itemsPerPage;
    const indexOfFirstItem = indexOfLastItem - itemsPerPage;
    const currentItems = features.slice(indexOfFirstItem, indexOfLastItem);
    const totalPages = Math.ceil(features.length / itemsPerPage);

    if (loading) return <p>Yükleniyor...</p>;

    return (
        <div style={{ marginTop: "20px" }}>
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
                    {currentItems.map((feature) => (
                        <tr key={feature.id}>
                            <td style={cellStyle}>{feature.id}</td>
                            <td style={cellStyle}>{feature.name}</td>
                            <td style={cellStyle}>{feature.wkt}</td>
                            <td style={cellStyle}>
                                <div style={{ display: "flex", gap: "10px" }}>
                                    <button onClick={() => handleDelete(feature.id)}>Sil</button>
                                    <button onClick={() => handleUpdate(feature)}>Güncelle</button>
                                </div>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
            <div style={{ marginTop: "10px", display: "flex", gap: "10px", justifyContent: "center" }}>
                <button onClick={() => setCurrentPage(prev => Math.max(prev - 1, 1))} disabled={currentPage === 1}>
                    &lt; Önceki
                </button>

                {/* Sayfa numaraları */}
                {Array.from({ length: totalPages }, (_, i) => (
                    <button
                        key={i}
                        onClick={() => setCurrentPage(i + 1)}
                        style={{
                            fontWeight: currentPage === i + 1 ? "bold" : "normal",
                            backgroundColor: currentPage === i + 1 ? "#d3d3d3" : "white",
                        }}
                    >
                        {i + 1}
                    </button>
                ))}

                <button onClick={() => setCurrentPage(prev => Math.min(prev + 1, totalPages))} disabled={currentPage === totalPages}>
                    Sonraki &gt;
                </button>
            </div>
        </div>
    );
};

const cellStyle = {
    border: "1px solid #ddd",
    padding: "8px",
    textAlign: "left",
    fontSize: "13px",
};

export default FeatureList;