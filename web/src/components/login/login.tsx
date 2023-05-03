import React from "react";
import { useSession, signIn, signOut } from "next-auth/react"
import { authOptions } from "./../../pages/api/auth/[...nextauth]"
import './login.module.css'

export const Login: React.FunctionComponent = () => {
    const { data: session } = useSession();
    if (session) {
        return (
            <div className="login">
                Hello {session.user.email}
                <button onClick={() => signOut()}>Logout</button>
            </div>
        );
    }

    return (
        <div className="login">
            Hello someone
            <button onClick={() => signIn()}>Login</button>
        </div>
    );
};