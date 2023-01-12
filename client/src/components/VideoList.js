import axios from "axios";
import React, { useEffect, useState } from "react";
import { IndividualVideo } from "./IndividualVideo";

export const VideoList = ({ handleCurrentVideo }) => {
  const token = localStorage.getItem("token");
  const [videos, setVideos] = useState([]);

  useEffect(() => {
    axios
      .get("https://localhost:5001/api/videos", {
        headers: {
          Authorization: `${token}`,
        },
      })
      .then((response) => setVideos(response.data))
      .catch((error) => console.log(error));
  }, []);

  return (
    <div className="list-group w-25">
      {videos.map((video) => (
        <IndividualVideo
          key={video.id}
          video={video}
          handleCurrentVideo={handleCurrentVideo}
        />
      ))}
    </div>
  );
};
