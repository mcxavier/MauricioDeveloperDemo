export const env = {
    port: process.env.PORT || 9000,
    axiosTimeout: process.env.AXIOS_TIMEOUT || 5000,
    API_BENEFICIOS_SEGURO: process.env.BENEFICIOS_SEGURO || 'http://bcpf-srv-cartoes-seguros.pfl2.svc.cluster.local',
    URL_SRV_GATEWAY_CARTOES_AUTENTICAR: process.env.URL_SRV_GATEWAY_CARTOES_AUTENTICAR || 'http://bcpf-srv-gateway-cartoes-autenticar.pfl2.svc.cluster.local',
    URL_SRV_LISTAR_CARTOES_CLIENTE: process.env.URL_SRV_LISTAR_CARTOES_CLIENTE || 'http://bcpf-srv-cartoes-listar.pfl2.svc.cluster.local',
    URL_SRV_LISTAR_VENCIMENTOS: process.env.URL_SRV_LISTAR_VENCIMENTOS || 'http://bcpf-srv-cartoes-consultar-faturas.pfl2.svc.cluster.local',
    URL_SRV_LISTAR_LANCAMENTOS: process.env.URL_SRV_LISTAR_LANCAMENTOS || 'http://bcpf-srv-cartoes-lanctos-faturas.pfl2.svc.cluster.local',
    URL_SRV_COTACAO_DOLAR: process.env.URL_SRV_COTACAO_DOLAR || 'http://bcpf-srv-cartoes-cotacao-dolar.pfl2.svc.cluster.local',
    URL_SRV_CARTOES_FATURA_PDF: process.env.URL_SRV_CARTOES_FATURA_PDF || 'http://bcpf-srv-cartoes-fatura-pdf.pfl2.svc.cluster.local',
    API_HOST_DADOS_CLIENTE: process.env.API_HOST_DADOS_CLIENTE || 'http://bcpf-srv-dados-cliente.pfl2.svc.cluster.local',
    NODE_ENV: process.env.NODE_ENV || 'production',
};