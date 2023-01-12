import axios from "axios";
import React, { useState } from "react";

export const AddVideo = () => {
  const [file, setFile] = useState();
  const token = localStorage.getItem("token");
  const formData = new FormData();
  formData.append("file", file);

  const handleChange = (event) => {
    setFile(event.target.files[0]);
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    axios
      .post("https://localhost:5001/api/videos", formData, {
        headers: {
          Authorization: `${token}`,
        },
      })
      .then(() => window.location.reload())
      .catch((error) => console.log(error));
  };

  return (
    <div className="container">
      <form onSubmit={handleSubmit}>
        <input type="file" onChange={handleChange} />
        <button type="submit" className="btn btn-primary">
          Add Video
        </button>
      </form>
    </div>
  );
};
