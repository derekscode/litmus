﻿using System;
using System.Linq;
using System.Net;
using System.Security.Principal;
using Litmus.Entities;
using Litmus.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Litmus.Controllers
{
    [Route("api/[controller]")]
    public class CardController : Controller
    {
        private ICardData _cardData;
        private ILogData _logData;

        public CardController(ICardData cardData, ILogData logData)
        {
            _cardData = cardData;
            _logData = logData;
        }

        // api/card/UserIsAdmin
        [HttpGet("UserIsAdmin")]
        public bool UserIsAdmin()
        {
            // if userIsUser and userIsAdmin are false, then: Stop solution, Clean, Rebuild, Run again
            //bool userIsUser = User.IsInRole(@"CORP\ecoATM Litmus User");
            bool userIsAdmin = HttpContext.User.IsInRole(@"CORP\ecoATM Litmus Admin");
            //bool shouldBeFalse = User.IsInRole(@"CORP\Fake Group");
            return userIsAdmin;
        }

        // GET: api/card
        [HttpGet]
        [Authorize(Roles = @"CORP\ecoATM Litmus User")]
        public Card[] Get()
        {
            var cards = _cardData.GetAll().ToList();

            var result = cards
                            .OrderByDescending(x => x.LastChanged);

            return result.ToArray();
        }

        // GET api/card/1
        [HttpGet("{id}")]
        public Card Get(int id)
        {
            var card = _cardData.Get(id);

            return card;
        }

        // POST api/card
        [HttpPost]
        public IActionResult Create([FromBody] Card card)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    card.Active = true;

                    _cardData.Add(card);
                    CreateNewLog(card, new Card());

                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(new { Data = card });
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }

            return Json("Failed");
        }

        // PUT api/card/1
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Card updatedCard)
        {
            if (updatedCard == null || updatedCard.Id != id)
            {
                return BadRequest();
            }

            Card oldCard = _cardData.Get(id).ShallowCopy();

            updatedCard.LastChanged = DateTime.Now;

            _cardData.Update(updatedCard);
            CreateNewLog(updatedCard, oldCard);

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json("Complete!");
        }

        //DELETE api/card/1
        //[Authorize(Roles = ActiveDirectory.Admin)]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Card oldCard = _cardData.Get(id).ShallowCopy();
            Card newCard = _cardData.Get(id).ShallowCopy();

            newCard.Active = false;

            _cardData.Update(newCard);
            CreateNewLog(newCard, oldCard);

            return Json("Complete!");
        }

        public void CreateNewLog(Card newCard, Card oldCard)
        {
            Log newLog = new Log()
            {
                CardIdNumber = newCard == null ? oldCard.IdNumber : newCard.IdNumber,
                CardId = oldCard.Id,
                DateChanged = DateTime.UtcNow,
                OldCard = JsonConvert.SerializeObject(oldCard),
                NewCard = JsonConvert.SerializeObject(newCard),
                User = WindowsIdentity.GetCurrent().Name
            };

            _logData.Add(newLog);
        }
    }
}
