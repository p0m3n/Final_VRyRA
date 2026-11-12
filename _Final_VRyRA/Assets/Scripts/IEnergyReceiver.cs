// Cualquier objeto que pueda recibir un disparo de energía implementa esta interfaz:
// runas, libros, bolitas del Lich, el Lich mismo...
// La bala no necesita saber QUÉ objeto es, solo que sabe recibir energía.
public interface IEnergyReceiver
{
    void ReceiveEnergy(EnergyType energy);
}
