import { Layout } from "@/components/layout/layout";
import React, { useEffect, useState } from "react";


export default function Welcome() {
    const [mighty, setMighty] = useState([]);
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
    }, []);
    return (
        <Layout>

            Data 1
            <br/>
            <p>{ mighty }</p>

        </Layout>
    )
}
