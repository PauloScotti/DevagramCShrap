﻿using DevagramCShrap.Models;

namespace DevagramCShrap.Repository
{
    public interface ICurtidaRepository
    {
        public void Curtir(Curtida curtida);
        public void Descurtir(Curtida curtida);
        public Curtida GetCurtida(int idPubicacao, int idUsuario);
        List<Curtida> GetCurtidaPorPublicacao(int idPublicacao);
    }
}
