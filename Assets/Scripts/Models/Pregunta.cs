[System.Serializable]
public class Pregunta
{
    public int idPregunta;
    public int idExamen;
    public string textoPregunta;
    public Respuesta[] respuestas;

    public Pregunta(int idPregunta, int idExamen, string textoPregunta, Respuesta[] respuestas)
    {
        this.idPregunta = idPregunta;
        this.idExamen = idExamen;
        this.textoPregunta = textoPregunta;
        this.respuestas = respuestas;
    }
}