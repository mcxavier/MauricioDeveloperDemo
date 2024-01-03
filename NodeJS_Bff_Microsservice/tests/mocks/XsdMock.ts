export default {
    open: {
        periferico: "456063730544101",
        idioma: 1,
        canal: 66,
        empresa: 237,
        uuid: "b7c356fb-2469-4725-aff1-fa86efd9c058",
        dependencia: 1,
        cliente: {
            dac: "4",
            tipoCliente: "F",
            cpfCnpj: "725223903000092",
            pj00: false,
            agencia: "3877",
            razao: "0705",
            titularidade: "1",
            conta: "15254",
            nomeCliente: "JULIANA SILVETRE DIAS",
            tipoConta: "FACIL",
        },
    },
    closed: {
        frwk: {
            ticket: "6BF94A2D109D81B5",
            idsessao: "0003C074B0561D1X02CC3A390B2F3E7E",
            usuario: "03864000000019070501",
            tipoUsuario: "CONTA",
        },
    },
    exportTo: (options: any) => {
        options["x-stateless-open"] = `ewogICJwZXJpZmVyaWNvIjogIjQ1NjA2MzczMDU0NDEwMSIsCiAgImlkaW9tYSI6IDEsCiAgImNhbmFsIjogNjYsCiAgImVtcHJlc2EiOiAyMzcsCiAgInV1aWQiOiAiYjdjMzU2ZmItMjQ2OS00NzI1LWFmZjEtZmE4NmVmZDljMDU4IiwKICAiZGVwZW5kZW5jaWEiOiAxLAogICJjbGllbnRlIjogewogICAgImRhYyI6ICI3IiwKICAgICJ0aXBvQ2xpZW50ZSI6ICJGIiwKICAgICJjcGZDbnBqIjogIjcyNTIyMzkwMzAwMDA5MiIsCiAgICAicGowMCI6IGZhbHNlLAogICAgImFnZW5jaWEiOiAiMzk5NSIsCiAgICAicmF6YW8iOiAiMDcwNSIsCiAgICAidGl0dWxhcmlkYWRlIjogIjEiLAogICAgImNvbnRhIjogIjI3MTk0OCIsCiAgICAibm9tZUNsaWVudGUiOiAiSlVMSUFOQSBTSUxWRVRSRSBESUFTIiwKICAgICJ0aXBvQ29udGEiOiAiRkFDSUwiCiAgfSwKICAiYWxnIjogIkVTMjU2Igp9`;
        options["x-stateless-closed"] = `eyJmcndrIjp7InRpY2tldCI6IjZCRjk0QTJEMTA5RDgxQjUiLCJpZHNlc3NhbyI6IjAwMDNDMDc0QjA1NjFEMVgwMkNDM0EzOTBCMkYzRTdFIiwidXN1YXJpbyI6IjAzODY0MDAwMDAwMDE5MDcwNTAxIiwidGlwb1VzdWFyaW8iOiJDT05UQSJ9fQ`;
    },
};
