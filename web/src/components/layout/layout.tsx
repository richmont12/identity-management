import React from "react";
import Head from 'next/head'
import { Menu } from "../menu/menu";
import { Header } from "../header/header";
import './layout.module.css'
type LayoutProps = React.PropsWithChildren;

export const Layout: React.FunctionComponent<LayoutProps> = (props) => {
    return (
        <>
            <Head>
                <title>Identity Management - Web</title>
                <meta name="description" content="Identity Management - Web" />
                <meta name="viewport" content="width=device-width, initial-scale=1" />
                <link rel="icon" href="/favicon.ico" />
            </Head>
            <main>
                <div className="main">
                    <Header></Header>
                    <Menu></Menu>
                    <div className="content">
                        {props.children}
                    </div>
                </div>
            </main>
        </>
    );
};