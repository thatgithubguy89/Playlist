import axios from "axios";
import { createContext } from "react";

const PlaylistContext = createContext();

export const PlaylistProvider = ({ children }) => {
  const token = localStorage.getItem("token");

  const handleUpdateVideoView = (videoId) => {
    axios
      .put(
        `https://localhost:5001/api/videos/${videoId}`,
        {},
        {
          headers: {
            Authorization: `${token}`,
          },
        }
      )
      .catch((error) => console.log(error));
  };

  const handleDeleteVideo = (videoId) => {
    axios
      .delete(`https://localhost:5001/api/videos/${videoId}`, {
        headers: {
          Authorization: `${token}`,
        },
      })
      .then(() => {
        window.location.reload();
      })
      .catch((error) => console.log(error));
  };

  return (
    <PlaylistContext.Provider
      value={{
        handleUpdateVideoView,
        handleDeleteVideo,
      }}
    >
      {children}
    </PlaylistContext.Provider>
  );
};

export default PlaylistContext;
