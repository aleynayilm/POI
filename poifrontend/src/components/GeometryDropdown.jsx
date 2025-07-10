import React, { useState } from "react";

const GeometryDropdown = ({ selected, setSelected }) => {
    const handleChange = (e) => {
        setSelected(e.target.value);
    };

    return (
        <div style={containerStyle}>
            <h2>GEOMETRİ EKLE</h2>
            <label htmlFor="geometry" style={labelStyle}>
                Geometri Türü:
            </label>
            <select
                id="geometry"
                value={selected}
                onChange={handleChange}
                style={selectStyle}
            >
                <option value=""> Seçiniz </option>
                <option value="Point">Point</option>
                <option value="Polygon">Polygon</option>
                <option value="LineString">LineString</option>
            </select>
        </div>
    );
};
const containerStyle = {
    margin: "20px",
    display: "flex",
    flexDirection: "column",
    maxWidth: "220px",
};

const labelStyle = {
    fontWeight: "bold",
    marginBottom: "8px",
    fontSize: "15px",
    color: "#333",
};

const selectStyle = {
    padding: "8px 10px",
    fontSize: "14px",
    borderRadius: "6px",
    border: "1px solid #ccc",
    outline: "none",
    backgroundColor: "#f9f9f9",
    cursor: "pointer",
    transition: "border-color 0.2s",
};

export default GeometryDropdown;
