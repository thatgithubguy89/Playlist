import axios from "axios";
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

export const Register = () => {
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
      .post("https://localhost:5001/api/Authentication/register", formData)
      .then(() => {
        navigation("/signin", { replace: true });
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
          <button type="submit" className="btn btn-primary mt-3">
            Register
          </button>
        </fieldset>
      </form>
    </div>
  );
};
