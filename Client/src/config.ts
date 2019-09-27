const dev = {
    api: {
        URL: "https://localhost:44319"
    },
};
  
const prod = {
    api: {
        URL: "http://localhost:44319"
    },
};

const config = process.env.NODE_ENV === 'development'
    ? dev
    : prod;

export default config;