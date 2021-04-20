FROM node:12.18-alpine
WORKDIR /api
COPY . .
RUN npm install
EXPOSE 8080
CMD ["npm", "run", "migrate-and-start"]