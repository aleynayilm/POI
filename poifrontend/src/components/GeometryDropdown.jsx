import React, { useState } from "react";

const GeometryDropdown = ({ selected, setSelected }) => {
    const handleChange = (e) => {
        setSelected(e.target.value);
    };

    return (
        <div style={{ margin: "20px" }}>
            <label htmlFor="geometry">GEOMETRİ TÜRÜ: </label>
            <select id="geometry" value={selected} onChange={handleChange}>
                <option value=""> Seçiniz </option>
                <option value="Point">Point</option>
                <option value="Polygon">Polygon</option>
                <option value="LineString">LineString</option>
            </select>

            {selected && <p>You selected: <strong>{selected}</strong></p>}
        </div>
    );
};

export default GeometryDropdown;
