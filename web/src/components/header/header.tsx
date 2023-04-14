import React from "react";
import './header.module.css'
import { Login } from "../login/login";

export const Header: React.FunctionComponent = () => {
    return (
        <div className="header">
            <div className="headerContent">
                <div className="headerLogo">
                </div>
                <div className="headerTitle">
                    <p>Identity Management - Web</p>
                </div>
                <Login/>
            </div>
        </div>
    );
};