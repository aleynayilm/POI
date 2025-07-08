import React, { useEffect, useState } from "react";

const MapForm = () => {
  const [names, setNames] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedName, setSelectedName] = useState("");
  const [wkt, setWkt] = useState("");

  useEffect(() => {
    fetch("https://localhost:5001/api/mapobject")
      .then((res) => res.json())
      .then((data) => {
        setNames(data);
        setLoading(false);
      })
      .catch((error) => {
        console.error("Veri alınırken hata oluştu:", error);
        setLoading(false);
      });
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();


    const response = await fetch("https://localhost:5001/api/mapobject", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ name: selectedName, wkt }),
    });

    const result = await response.json();

    if (response.ok) {
      alert("Veri başarıyla eklendi!");
      setSelectedName("");
      setWkt("");
    } else {
      alert("Hata: " + result.message);
    }
  };
  return (
    <form onSubmit={handleSubmit} className="form-box">
      <label className="label">Şehir Adı:  </label>
      <input
        className="input"
        list="name-options"
        value={selectedName}
        onChange={(e) => setSelectedName(e.target.value)}
        required
      />
      <datalist id="name-options">
        {names.map((item) => (
          <option key={item.id} value={item.name} />
        ))}
      </datalist>

      <label className="label">WKT (Well-Known Text):  </label>
      <textarea
        className="input"
        value={wkt}
        onChange={(e) => setWkt(e.target.value)}
        required
      />

      <button type="submit" className="submit-button">
        Kaydet
      </button>

      {loading && <div className="loader">Yükleniyor...</div>}
    </form>
  );
};

export default MapForm;



