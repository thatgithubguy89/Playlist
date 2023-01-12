import React, { useState } from "react";
import axios from "axios";
import { Link, useNavigate } from "react-router-dom";

export const Signin = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const navigation = useNavigate();

  const handleSubmit = (e) => {
    e.preventDefault();

    const formData = {
      username,
      password,
    };

    axios
      .post("https://localhost:5001/api/Authentication/signin", formData)
      .then((response) => {
        localStorage.setItem("token", `Bearer ${response.data}`);
        navigation("/home", { replace: true });
        window.location.reload();
      })
      .catch((error) => console.log(error));
  };

  return (
    <div className="container w-25 mt-5">
      <form onSubmit={handleSubmit}>
        <fieldset>
          <div className="form-group">
            <input
              type="email"
              className="form-control"
              placeholder="Enter email"
              onChange={(e) => setUsername(e.target.value)}
            />
          </div>
          <div className="form-group mt-3">
            <input
              type="password"
              className="form-control"
              placeholder="Password"
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>
          <button type="submit" className="btn btn-primary mt-3 mb-3">
            Signin
          </button>
          <br />
          <Link to={"/register"}>No account? Register here.</Link>
        </fieldset>
      </form>
    </div>
  );
};
