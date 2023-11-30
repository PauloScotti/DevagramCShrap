﻿using DevagramCShrap.Models;

namespace DevagramCShrap.Repository.Impl
{
    public class SeguidorRepositoryImpl : ISeguidorRepository
    {
        private readonly DevagramContext _context;

        public SeguidorRepositoryImpl(DevagramContext context)
        {
            _context = context;
        }
        public bool Seguir(Seguidor seguidor)
        {
            try
            {
                _context.Add(seguidor);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Desseguir(Seguidor seguidor)
        {
            try
            {
                _context.Remove(seguidor);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Seguidor GetSeguidor(int idseguidor, int idseguido)
        {
            return _context.Seguidores.FirstOrDefault(s => s.IdUsuarioSeguidor == idseguidor && s.IdUsuarioSeguido == idseguido);
        }
    }
}