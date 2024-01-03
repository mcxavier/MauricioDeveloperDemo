import express from "express";
import swaggerUi from "swagger-ui-express";
import specs from "../swagger/Swagger";
import HealthController from "../controllers/HealthController";
import ListarCartoesController from "../controllers/ListarCartoesController";
import DadosSeguroController from "../controllers/DadosSeguroController";
import BeneficiosSeguroController from "../controllers/BeneficiosSeguroController";


const router = express.Router();

// Documentação
router.use("/api-docs", swaggerUi.serve);
router.get("/api-docs", swaggerUi.setup(specs, { explorer: true }));

const healthController = new HealthController();
router.route('/health').get(healthController.check)

const listarCartoesController = new ListarCartoesController();
router.route('/listarCartoes').get(listarCartoesController.listarCartoesSeguro)

const dadosSeguro = new DadosSeguroController();
router.route('/dadosSeguro').post(dadosSeguro.listaDadosSeguro)

const contratoSeguro = new DadosSeguroController();
router.route('/contratarSeguro').post(contratoSeguro.contratacaoSeguro)

const cancelarSeguro = new DadosSeguroController();
router.route('/cancelarSeguro').post(cancelarSeguro.cancelamentoSeguro)

const beneficiosSeguro = new BeneficiosSeguroController();
router.route('/listarBeneficiosSeguro').get(beneficiosSeguro.listaBeneficiosSeguro)

export = router;
