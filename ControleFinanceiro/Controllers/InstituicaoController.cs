﻿using ControleFinanceiro.Data;
using ControleFinanceiro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Controllers
{
    public class InstituicaoController : Controller
    {   
        private readonly AcademicoContext _context;

        public InstituicaoController(AcademicoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Instituicoes.OrderBy(i => i.Nome).ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost] /* formuláio */
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Nome", "Endereco")]Instituicao instituicao) /*sobrecarga */
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(instituicao);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch(DbUpdateException ex)
            {
                ModelState.AddModelError("Erro de cadastro", "Não foi possível cadastrar a instituição.");
            }
            return View(instituicao);
        }
        public async Task<ActionResult> Edit(long id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var instituicao = await _context.Instituicoes.SingleOrDefaultAsync(i => i.InstituicaoId == id);
            if (instituicao == null)
            {
                return NotFound();
            }
            return View(instituicao);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, [Bind("InstituicaoId", "Nome", "Endereco")] Instituicao instituicao)
        {
            if (id != instituicao.InstituicaoId)
            {   
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instituicao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex) 
                {
                    if (!InstituicaoExists(instituicao.InstituicaoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(instituicao);
        }

        private bool InstituicaoExists(long? instituicaoId)
        {
            var instituicao = _context.Instituicoes.FirstOrDefault(i => i.InstituicaoId == instituicaoId);
            if (instituicao == null)
                return false;
            return true;
        }
        public async Task<ActionResult> Details(long id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var instituicao = await _context.Instituicoes.SingleOrDefaultAsync(i => i.InstituicaoId == id);
            if (instituicao == null)
            {
                return NotFound();
            }
            return View(instituicao);
        }
        public async Task<ActionResult> Delete(long id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var instituicao = await _context.Instituicoes.SingleOrDefaultAsync(i => i.InstituicaoId == id);
            if (instituicao == null)
            {
                return NotFound();
            }
            return View(instituicao);
        }

        [HttpPost, ActionName("Delete")] /*formulário, vem de objeto */
        [ValidateAntiForgeryToken] /*todo site tem seu token, ele recebe esse token ele verifica se está correto*/
        public async Task<IActionResult> DeleteConfirmed(long? id) /*sobrecarga */
        {
            var instituicao = await _context.Instituicoes.SingleOrDefaultAsync(i => i.InstituicaoId == id);
            _context.Instituicoes.Remove(instituicao);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
