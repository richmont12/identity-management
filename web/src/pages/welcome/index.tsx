import { Layout } from "@/components/layout/layout";
import React, { useEffect, useState } from "react";


export default function Welcome() {
    const [mighty, setMighty] = useState([]);
    const [mighty2, setMighty2] = useState([]);
    useEffect(() => {
        fetch('http://localhost:3000/api/mightydata')
            .then((response) => response.json())
            .then((data) => {
                setMighty(data.mightyData);
            })
            .catch((err) => {
                setMighty("you are not authorized for mighty1");
                console.log(err.message);
            });

        fetch('http://localhost:3000/api/mightydata2')
            .then((response) => response.json())
            .then((data) => {
                setMighty(data.mightyData2);
            })
            .catch((err) => {
                setMighty("you are not authorized for mighty2");
                console.log(err.message);
            });
    }, []);
    return (
        <Layout>

            Welcome to Identity Management Web

            <p>{ mighty }</p>
            <hr></hr>
            <p>{ mighty2 }</p>

        </Layout>
    )
}
