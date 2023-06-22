import { getToken } from "next-auth/jwt"

export default async function handler(req: any, res: any) {
    const token = await getToken({ req })
    if (token) {
        // Signed in
        let mightyData = await httpGet(token.access_token, '/api/backend/1/mighty2')
        res.status(200).json({ text: 'mighty-data-next-js', mightyData: mightyData });
    } else {
        // Not Signed in
        res.status(401)
    }
    res.end()
}


const http = require('https');
const bl = require('bl');

async function httpGet(token: string, path: string) {
    return new Promise((resolve, reject) => {
        const options = {
            hostname: 'localhost',
            path: path,
            port: 6001,
            method: 'GET',
            headers: {
                Authorization: 'Bearer ' + token
            }

        };
        
        http.get(options, (response: { setEncoding: (arg0: string) => void; pipe: (arg0: any) => void; }) => {
            response.setEncoding('utf8');
            response.pipe(bl((err: any, data: { toString: () => unknown; }) => {
                if (err) {
                    reject(err);
                }
                resolve(data.toString());
            }));
        });
    });
}