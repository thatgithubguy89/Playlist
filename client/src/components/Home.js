import React, { useState } from "react";
import { AddVideo } from "./AddVideo";
import { VideoList } from "./VideoList";
import { VideoPlayer } from "./VideoPlayer";

export const Home = () => {
  const [videoUrl, setVideoUrl] = useState("");

  const handleCurrentVideo = (url) => {
    setVideoUrl("https://localhost:5001" + url);
  };

  return (
    <div>
      <div className="container mt-5 mb-5" style={{ display: "flex" }}>
        <VideoPlayer videoUrl={videoUrl} />
        <VideoList handleCurrentVideo={handleCurrentVideo} />
      </div>
      <AddVideo />
    </div>
  );
};
