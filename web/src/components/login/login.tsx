import React from "react";
import './login.module.css'
export const Login: React.FunctionComponent = () => {
    return (
        <div className="login">
            <input type="text" placeholder="Enter Username" name="uname" required />

            <input type="password" placeholder="Enter Password" name="psw" required />

            <button type="submit">Login</button>
        </div>
    );
};