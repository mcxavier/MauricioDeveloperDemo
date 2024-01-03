FROM nexusrepository.bradesco.com.br:8500/rhscl/nodejs-14-rhel7:1-24

WORKDIR bcpf-bff-cartoes-seguros

COPY . .

EXPOSE 8080
 
CMD ["node", "./dist/index.js"]