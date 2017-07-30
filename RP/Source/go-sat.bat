@ECHO ----------------------------------------------------------------------
@ECHO --- Se crean aqui los RO a partir de los archibos .gra .grxml o .sjv 
@ECHO Compile the recognition object using the SATCA configuration
@ECHO file saludos.hdr. The copiled RO is created here (-o .)
@ECHO and previously created RO are overwritten (-s)
@ECHO ----------------------------------------------------------------------
@ECHO LoqASRsatca -a cancel.hdr -o . -s -z
@ECHO LoqASRsatca -a despedida.hdr -o . -s -z
@ECHO LoqASRsatca -a eating.hdr -o . -s -z
@ECHO LoqASRsatca -a habilidad.hdr -o . -s -z
@ECHO LoqASRsatca -a hora.hdr -o . -s -z
@ECHO LoqASRsatca -a nombres.hdr -o . -s -z
@ECHO LoqASRsatca -a saludos.hdr -o . -s -z
@ECHO LoqASRsatca -a tiempo.hdr -o . -z
@ECHO LoqASRsatca -a tipocomida.hdr -o . -s -z
@ECHO LoqASRsatca -a rest.hdr -o . -z
@ECHO LoqASRsatca -a restaurantes.hdr -o . -z
LoqASRsatca -a encuesta.hdr -o . -z

@ECHO ----------------------------------------------------------------------
@ECHO Compile the recognition package. The copiled RP is created here
@ECHO (-o .), including the saludos RO and overwriting previously
@ECHO created RP (-s)
@ECHO ----------------------------------------------------------------------
@ECHO LoqASRsatca -t rp -o . -n rpperson -r person -s
