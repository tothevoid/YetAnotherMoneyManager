FROM node:22.13-alpine3.20 as build
WORKDIR /app
COPY package.json /app/package.json
RUN npm install --only=prod
COPY . /app
ARG SERVER_URL
ENV REACT_APP_SERVER_URL $SERVER_URL
RUN npm run build
FROM nginx:1.16.0-alpine
COPY --from=build /app/build /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]