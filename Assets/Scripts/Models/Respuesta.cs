[System.Serializable]
public class Respuesta
{
    public int idRespuesta;
    public string textoRespuesta;
    public int esCorrecta;
    public int idPregunta;

    public Respuesta(int idRespuesta, string textoRespuesta, int esCorrecta, int idPregunta)
    {
        this.idRespuesta = idRespuesta;
        this.textoRespuesta = textoRespuesta;
        this.esCorrecta = esCorrecta;
        this.idPregunta = idPregunta;
    }
}