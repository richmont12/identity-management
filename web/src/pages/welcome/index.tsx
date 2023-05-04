import { Layout } from "@/components/layout/layout";
import React, { useEffect, useState } from "react";


export default function Welcome() {
    const [mighty, setMighty] = useState([]);
    useEffect(() => {
        fetch('http://localhost:3000/api/mightydata')
            .then((response) => response.json())
            .then((data) => {
                console.log(data);
                setMighty(data.text);
            })
            .catch((err) => {
                setMighty("you are not authorized");
                console.log(err.message);
            });
    }, []);
    return (
        <Layout>

            Welcome to Identity Management Web

            <p>{ mighty }</p>

        </Layout>
    )
}
