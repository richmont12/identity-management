import React from "react";
import Head from 'next/head'
import { useSession } from "next-auth/react";

export const Menu: React.FunctionComponent = () => {
    const { data: session } = useSession();

    if(session) {
        return (
            <div className="menu">
                <ul>
                    <li>
                        <a href="/">Home</a>
                    </li>
                    <li>
                        <a href="data1">data1</a>
                    </li>
                    <li>
                        <a href="data2">data2</a>
                    </li>
                </ul>
            </div>
        );
    }

    
    return (
        <div className="menu">
            <ul>
                <li>
                    <a href="/">Home</a>
                </li>
            </ul>
        </div>
    );
};