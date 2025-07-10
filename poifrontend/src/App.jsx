import React, { useState } from "react";
import GeometryDropdown from "./components/GeometryDropdown";
import AccessibleMap from "./components/AccessibleMap";
import FeatureList from "./components/FeatureList";

function App() {
    const [geometryType, setGeometryType] = useState("");
    return (
        <div style={{ display: "flex", gap: "30px" }}>
            <div>
                <h1>GEOMETRY EKLE</h1>
                <GeometryDropdown selected={geometryType} setSelected={setGeometryType} />
                <AccessibleMap geometryType={geometryType} />
            </div>
            <div>
                <FeatureList />
            </div>
        </div>
    );
}
export default App;
