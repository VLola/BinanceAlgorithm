﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BinanceAlgorithm.Resourses.History;

namespace BinanceAlgorithm.Resourses
{
    public static class ResultPercentEma
    {
        public static ListEma ResultListEma(ListEma list_ema_long, ListEma list_ema_short)
        {
            List<decimal> list_decimal = new List<decimal>();
            for (int j = 0; j < list_ema_long.list.Count; j++)
            {
                decimal a = list_ema_long.list[j];
                decimal b = list_ema_short.list[j];
                decimal result;
                if (a == b) result = 0m;
                else result = (a - b) / b * 100;
                list_decimal.Add(Math.Round(result, 2));
            }
            return new ListEma(list_ema_long.Sumbol, list_decimal);
        }
        public static History ResultHistory(ListEma list_ema, decimal start, decimal tp, decimal sl)
        {
            //list_ema.list.Reverse();
            int long_bet = 0;
            int short_bet = 0;
            int long_win = 0;
            int short_win = 0;
            int long_loss = 0;
            int short_loss = 0;
            List<MovementHistory> movement_history = new List<MovementHistory>();
            for (int i = 0; i < list_ema.list.Count; i++)
            {
                MovementHistory m_history = new MovementHistory();
                if (list_ema.list[i] > start)
                {
                    decimal bet = list_ema.list[i];

                    for (int j = i; j < list_ema.list.Count; j++)
                    {
                        if (list_ema.list[j] > bet + sl)          // + или - , < или >
                        {
                            m_history.X1 = i;
                            m_history.X2 = j;
                            m_history.isPositive = false;
                            short_bet++;
                            short_loss++;
                            i = j;
                            break;
                        }
                        else if (list_ema.list[j] < bet - tp)          // + или - , < или >
                        {
                            m_history.X1 = i;
                            m_history.X2 = j;
                            m_history.isPositive = true;
                            short_bet++;
                            short_win++;
                            i = j;
                            break;
                        }
                    }
                }
                else if (list_ema.list[i] < -start)
                {
                    decimal bet = list_ema.list[i];
                    for (int j = i; j < list_ema.list.Count; j++)
                    {
                        if (list_ema.list[j] < bet - sl)          // + или - , < или >
                        {
                            m_history.X1 = i;
                            m_history.X2 = j;
                            m_history.isPositive = false;
                            long_bet++;
                            long_loss++;
                            i = j;
                            break;
                        }
                        else if (list_ema.list[j] > bet + tp)          // + или - , < или >
                        {
                            m_history.X1 = i;
                            m_history.X2 = j;
                            m_history.isPositive = true;
                            long_bet++;
                            long_win++;
                            i = j;
                            break;
                        }
                    }
                }
                movement_history.Add(m_history);
            }
            History history = new History(list_ema.Sumbol, long_bet, short_bet, long_win, short_win, long_loss, short_loss);
            //movement_history.Reverse();
            history.movement_history = movement_history;
            return history;
        }
    }
}