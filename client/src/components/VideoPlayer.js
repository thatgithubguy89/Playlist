export const VideoPlayer = ({ videoUrl }) => {
  return (
    <div
      style={{
        border: "1px solid black",
        radius: "5px",
        height: "500px",
        width: "800px",
      }}
    >
      <iframe src={videoUrl} style={{ width: "100%", height: "100%" }}></iframe>
    </div>
  );
};
