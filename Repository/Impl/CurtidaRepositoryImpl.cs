﻿using DevagramCShrap.Models;

namespace DevagramCShrap.Repository.Impl
{
    public class CurtidaRepositoryImpl : ICurtidaRepository
    {
        private readonly DevagramContext _context;

        public CurtidaRepositoryImpl (DevagramContext context)
        {
            _context = context;
        }

        public void Curtir(Curtida curtida)
        {
            _context.Add(curtida);
            _context.SaveChanges();
        }

        public void Descurtir(Curtida curtida)
        {
            _context.Remove(curtida);
            _context.SaveChanges();
        }

        public Curtida GetCurtida(int idPubicacao, int idUsuario)
        {
            return _context.Curtidas.FirstOrDefault(c => c.IdPublicacao == idPubicacao && c.IdUsuario == idUsuario);
        }

        public List<Curtida> GetCurtidaPorPublicacao(int idPublicacao)
        {
            return _context.Curtidas.Where(c => c.IdPublicacao == idPublicacao).ToList();
        }
    }
}
