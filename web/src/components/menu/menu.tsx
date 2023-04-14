import React from "react";
import Head from 'next/head'

export const Menu: React.FunctionComponent = () => {
    return (
        <div className="menu">
            <ul>
                <li>
                    <a href="welcome">Welcome</a>
                </li>
                <li>
                    <a href="/">Home</a>
                </li>
            </ul>
        </div>
    );
};