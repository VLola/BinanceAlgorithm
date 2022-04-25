using System;
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
        public static History ResultHistory(ListKlines list_klines, ListEma list_ema, decimal start, decimal tp, decimal sl)
        {
            tp = tp / 100;
            sl = sl / 100;
            int long_bet = 0;
            int short_bet = 0;
            int long_win = 0;
            int short_win = 0;
            int long_loss = 0;
            int short_loss = 0;
            List<MovementHistory> movement_history = new List<MovementHistory>();
            if (list_klines.symbol == list_ema.Sumbol)
            {
                for (int i = list_ema.list.Count - 1; i > 0; i--)
                {
                    MovementHistory m_history = new MovementHistory();
                    if (list_ema.list[i] > start)
                    {
                        decimal bet = (list_klines.listKlines[i].High + list_klines.listKlines[i].Low) / 2;
                        
                        for (int j = i - 1; j > 0; j--)
                        {
                            if (list_klines.listKlines[j].Low < bet - (bet * sl))          // + или - , < или >
                            {
                                m_history.X1 = i;
                                m_history.X2 = j;
                                m_history.Y1 = bet;
                                m_history.Y2 = bet - (bet * sl);
                                m_history.isPositive = false;
                                m_history.isLongPeriod = true;
                                movement_history.Insert(0, m_history);
                                long_bet++;
                                long_loss++;
                                i = j;
                                break;
                            }
                            else if (list_klines.listKlines[j].High > bet + (bet * tp))          // + или - , < или >
                            {
                                m_history.X1 = i;
                                m_history.X2 = j;
                                m_history.Y1 = bet;
                                m_history.Y2 = bet + (bet * tp);
                                m_history.isPositive = true;
                                m_history.isLongPeriod = true;
                                movement_history.Insert(0, m_history);
                                long_bet++;
                                long_win++;
                                i = j;
                                break;
                            }
                            else if (list_klines.listKlines[j].Low < bet - (bet * sl) && list_klines.listKlines[j].High > bet + (bet * tp)) break;
                        }
                    }
                    else if (list_ema.list[i] < -start)
                    {
                        decimal bet = (list_klines.listKlines[i].High + list_klines.listKlines[i].Low) / 2;
                        
                        for (int j = i - 1; j > 0; j--)
                        {
                            if (list_klines.listKlines[j].High > bet + (bet * sl))          // + или - , < или >
                            {
                                m_history.X1 = i;
                                m_history.X2 = j;
                                m_history.Y1 = bet;
                                m_history.Y2 = bet + (bet * sl);
                                m_history.isPositive = false;
                                m_history.isLongPeriod = false;
                                movement_history.Insert(0, m_history);
                                short_bet++;
                                short_loss++;
                                i = j;
                                break;
                            }
                            else if (list_klines.listKlines[j].Low < bet - (bet * sl))          // + или - , < или >
                            {
                                m_history.X1 = i;
                                m_history.X2 = j;
                                m_history.Y1 = bet;
                                m_history.Y2 = bet - (bet * sl);
                                m_history.isPositive = true;
                                m_history.isLongPeriod = false;
                                movement_history.Insert(0, m_history);
                                short_bet++;
                                short_win++;
                                i = j;
                                break;
                            }
                            else if (list_klines.listKlines[j].High > bet + (bet * sl) && list_klines.listKlines[j].Low < bet - (bet * sl)) break;
                        }
                    }
                    
                }

            }
            History history = new History(list_ema.Sumbol, long_bet, short_bet, long_win, short_win, long_loss, short_loss);
            history.movement_history = movement_history;
            return history;
        }
    }
}
