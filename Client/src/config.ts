const dev = {
    api: {
        URL: "https://localhost:2500"
    },
};
  
const prod = {
    api: {
        URL: "http://localhost:44319"
    },
};

const config = (process && process.env && process.env.REACT_APP_STAGE === "production")
    ? prod
    : dev;

export default config;