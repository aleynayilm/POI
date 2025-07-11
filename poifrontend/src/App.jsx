import React, { useState } from "react";
import GeometryDropdown from "./components/GeometryDropdown";
import AccessibleMap from "./components/AccessibleMap";
import FeatureList from "./components/FeatureList";
import GeometryViewer from "./components/GeometryViewer";

function App() {
    const [geometryType, setGeometryType] = useState("");
    const [refresh, setRefresh] = useState(false);
    const resetGeometryType = () => setGeometryType("");
    return (
        <div style={{ display: "flex", flexDirection: "column", gap: "30px", padding: "20px" }}>
            <div>
                <GeometryDropdown selected={geometryType} setSelected={setGeometryType} />
            </div>
            <AccessibleMap
                geometryType={geometryType}
                onRefresh={() => setRefresh((prev) => !prev)}
                refresh={refresh}
                onResetGeometryType={resetGeometryType}
            />
            <FeatureList
                refresh={refresh}
                onRefresh={() => setRefresh((prev) => !prev)}
            />
        </div>
    );
}
export default App;
