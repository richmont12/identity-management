import { getToken } from "next-auth/jwt"

export default async function handler(req, res) {
    const token = await getToken({ req })
    if (token) {
        // Signed in
        console.log("JSON Web Token", JSON.stringify(token, null, 2))
        //let data = await httpGet("https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY", token)

        let mightyData = await httpGet(token.access_token)
        console.log(mightyData)
        res.status(200).json({ text: 'mighty-data-next-js', mightyData: mightyData });
    } else {
        // Not Signed in
        res.status(401)
    }
    res.end()
}



const http = require('https');
const bl = require('bl');

async function httpGet(token) {
    return new Promise((resolve, reject) => {
        const options = {
            hostname: 'localhost',
            path: '/api/backend/1/mighty',
            port: 7262,
            method: 'GET',
            headers: {
                Authorization: 'Bearer ' + token
            }

        };
        
        http.get(options, response => {
            response.setEncoding('utf8');
            response.pipe(bl((err, data) => {
                
                console.log(err)
                if (err) {
                    console.log(err)
                    reject(err);
                }
                resolve(data.toString());
            }));
        });
    });
}