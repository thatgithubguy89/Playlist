import React from "react";
import { useNavigate } from "react-router-dom";

export const NavigationBar = () => {
  const navigation = useNavigate();

  const handleSignout = () => {
    localStorage.setItem("token", "");
    navigation("/signin", { replace: true });
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-light bg-light">
      <div className="container-fluid">
        <a className="navbar-brand" href="#">
          Playlist
        </a>
        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarColor03"
          aria-controls="navbarColor03"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarColor03">
          {localStorage.getItem("token") && (
            <ul className="navbar-nav me-auto">
              <li className="nav-item">
                <a className="nav-link" href="/home">
                  Home
                </a>
              </li>
              <li className="nav-item">
                <a className="nav-link" href="#" onClick={handleSignout}>
                  Sign Out
                </a>
              </li>
            </ul>
          )}
          {!localStorage.getItem("token") && (
            <ul className="navbar-nav me-auto">
              <li className="nav-item">
                <a className="nav-link" href="/signin">
                  Sign In
                </a>
              </li>
            </ul>
          )}
        </div>
      </div>
    </nav>
  );
};
