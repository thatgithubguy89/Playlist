import { BrowserRouter, Route, Routes } from "react-router-dom";
import "./index.css";
import { NavigationBar } from "./components/NavigationBar";
import { Signin } from "./components/Signin";
import { Register } from "./components/Register";
import { Home } from "./components/Home";
import { PlaylistProvider } from "./context/PlaylistContext";

function App() {
  return (
    <div className="App">
      <PlaylistProvider>
        <BrowserRouter>
          <NavigationBar />
          <Routes>
            <Route path="/home" element={<Home />} />
            <Route path="/signin" element={<Signin />} />
            <Route path="/register" element={<Register />} />
          </Routes>
        </BrowserRouter>
      </PlaylistProvider>
    </div>
  );
}

export default App;
