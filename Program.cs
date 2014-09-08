using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DerivativeV2._4
{
    public class Term
    {
        public double coefficient;
        public double power;
        public static List<Term> CreateTermsFromStringToList(String polynomial)
        {
            Char[] replacements = { 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm' };
            String[] replacementsAndPower = { "q^", "w^", "e^", "r^", "t^", "y^", "u^", "i^", "o^", "p^", "a^", "s^", "d^", "f^", "g^", "h^", "j^", "k^", "l^", "z^", "x^", "c^", "v^", "b^", "n^", "m^" };
            String[] replacementsForNoPower = { "q+", "w+", "e+", "r+", "t+", "y+", "u+", "i+", "o+", "p+", "a+", "s+", "d+", "f+", "g+", "h+", "j+", "k+", "l+", "z+", "x+", "c+", "v+", "b+", "n+", "m+" };
            String[] replacementsForNoPowerAndNegative = { "q-", "w-", "e-", "r-", "t-", "y-", "u-", "i-", "o-", "p-", "a-", "s-", "d-", "f-", "g-", "h-", "j-", "k-", "l-", "z-", "x-", "c-", "v-", "b-", "n-", "m-" };
            for (int i = 0; i < 26; i++)
            {
                polynomial = polynomial.Replace(replacementsAndPower[i], ",");
                polynomial = polynomial.Replace(replacementsForNoPower[i], ",1,");
                polynomial = polynomial.Replace(replacementsForNoPowerAndNegative[i], ",1,-");
                polynomial = polynomial.Replace(replacements[i], ',');
            }
            polynomial = polynomial.Replace(" + ", ",");
            polynomial = polynomial.Replace(" +", ",");
            polynomial = polynomial.Replace("+ ", ",");
            polynomial = polynomial.Replace("+", ",");
            polynomial = polynomial.Replace(" - ", ",-");
            polynomial = polynomial.Replace(" -", ",-");
            polynomial = polynomial.Replace("- ", ",-");
            polynomial = polynomial.Replace("-", ",-");
            polynomial = polynomial.Trim(',');
            polynomial = polynomial.Trim('(');
            polynomial = polynomial.Trim(')');
            string[] BasePolynomialArray = polynomial.Split(',');
            List<Term> termListOfPolynomialTerms = new List<Term>();
            for (int i = 0; i < BasePolynomialArray.Length; i = i + 2)
            {
                Term termToAdd = new Term();
                termToAdd.coefficient = Convert.ToDouble(BasePolynomialArray[i]);
                if (i + 1 <= BasePolynomialArray.Length - 1)
                {
                    termToAdd.power = Convert.ToDouble(BasePolynomialArray[i + 1]);
                }
                else
                {
                    termToAdd.power = (Double)0;
                }
                termListOfPolynomialTerms.Add(termToAdd);
            }
            return termListOfPolynomialTerms;
        }
        public static List<Term> PowerRuleDifferentiate(List<Term> listOfTerms)
        {
            List<Term> differentiatedTerms = new List<Term>();
            List<Term> shieldedListOfTerms = new List<Term>();
            foreach (Term shieldedTerm in listOfTerms)
            {
                Term tempTerm = new Term();
                tempTerm.coefficient = shieldedTerm.coefficient;
                tempTerm.power = shieldedTerm.power;
                shieldedListOfTerms.Add(tempTerm);
            }
            foreach (Term term in shieldedListOfTerms)
            {
                term.coefficient = term.coefficient * term.power;
                term.power = term.power - 1;
                if (term.coefficient != 0)
                {
                    differentiatedTerms.Add(term);
                }
            }
            return differentiatedTerms;
        }
        public static String PowerRuleDifferentiateStringReturn(String expression)
        {
            List<Term> listOfTermsToDifferentiate = new List<Term>(Term.CreateTermsFromStringToList(expression));
            List<Term> differentiatedListOfTerms = new List<Term>(Term.PowerRuleDifferentiate(listOfTermsToDifferentiate));
            string stringToReturn = "";
            foreach (Term term in differentiatedListOfTerms)
            {
                if (term.power == 0)
                {
                    stringToReturn = stringToReturn + term.coefficient + " ";
                }
                else
                {
                    stringToReturn = stringToReturn + term.coefficient + "x^" + term.power + " ";
                }
            }
            return stringToReturn;
        }
        public static List<Term> CombineLikeTerms(List<Term> listOfTerms)
        {
            int listOfTermsCount = listOfTerms.Count;
            Term termToAddMake = new Term();
            termToAddMake.coefficient = 0;
            termToAddMake.power = 0;
            List<Term> combinedTerms = new List<Term>();
            Term nullTerm = new Term();
            nullTerm.coefficient = 0;
            nullTerm.power = 0;
            Term reportedTerm = new Term();
            for (int i = 0; i < listOfTermsCount; i++)
            {
                Term termForComparison = new Term();
                termForComparison.power = listOfTerms[i].power;
                termToAddMake.power = termForComparison.power;
                termToAddMake.coefficient = 0;
                for (int p = 0; p < listOfTermsCount; p++)
                {
                    if (termForComparison.power == listOfTerms[p].power)
                    {
                        termToAddMake.coefficient = termToAddMake.coefficient + listOfTerms[p].coefficient;
                        listOfTerms.Insert(p, nullTerm);
                        listOfTerms.RemoveAt(p + 1);
                    }
                }
                if (termToAddMake.coefficient != 0)
                {
                    reportedTerm.power = termToAddMake.power;
                    reportedTerm.coefficient = termToAddMake.coefficient;
                    Term term2 = new Term();
                    term2.coefficient = reportedTerm.coefficient;
                    term2.power = reportedTerm.power;
                    combinedTerms.Add(term2);
                }
            }
            return combinedTerms;
        }
        public static List<Term> PolynomialMultiplication(List<Term> product1, List<Term> product2)
        {
            int termsInProduct1 = product1.Count;
            int termsInProduct2 = product2.Count;
            List<Term> newTermListNotCombined = new List<Term>();
            for (int i = 0; i < termsInProduct1; i++)
            {
                foreach (Term term in product2)
                {
                    newTermListNotCombined.Add(TermMultiply(product1[i], term));
                }
            }
            for (int i = 0; i < newTermListNotCombined.Count; i++)
            {
                if (newTermListNotCombined[i].coefficient == 0)
                {
                    newTermListNotCombined.Remove(newTermListNotCombined[i]);
                }
            }
            List<Term> newTermListCombined = new List<Term>(CombineLikeTerms(newTermListNotCombined));
            return newTermListCombined;
        }
        private static Term TermMultiply(Term term1, Term term2)
        {
            Term term3 = new Term();
            term3.coefficient = term1.coefficient * term2.coefficient;
            term3.power = term1.power + term2.power;
            return term3;
        }
        public static List<Term> MakePolynomialNegative(List<Term> listOfTerms)
        {
            List<Term> negativeListOfTerms = new List<Term>();
            foreach (Term term in listOfTerms)
            {
                term.coefficient = term.coefficient * -1;
                negativeListOfTerms.Add(term);
            }
            return negativeListOfTerms;
        }
        public static List<Term> ProductRuleDifferentiationListReturn(String expression)
        {
            expression = expression.Replace('*', ',');
            expression = expression.Replace(")(", ",");
            expression = expression.Trim('(');
            expression = expression.Trim(')');
            string[] expressionArray = expression.Split(',');
            List<Term> firstPolynomialRaw = new List<Term>(Term.CreateTermsFromStringToList(expressionArray[0]));
            List<Term> secondPolynomialRaw = new List<Term>(Term.CreateTermsFromStringToList(expressionArray[1]));
            List<Term> firstPolynomial = new List<Term>();
            List<Term> secondPolynomial = new List<Term>();
            firstPolynomial.AddRange(firstPolynomialRaw);
            secondPolynomial.AddRange(secondPolynomialRaw);
            List<Term> firstPolynomial2 = new List<Term>();
            List<Term> secondPolynomial2 = new List<Term>();
            firstPolynomial2.AddRange(firstPolynomialRaw);
            secondPolynomial2.AddRange(secondPolynomialRaw);
            //now differentiate the two equations:
            List<Term> differentiatedFirstPolynomial = new List<Term>();
            differentiatedFirstPolynomial.AddRange(Term.PowerRuleDifferentiate(firstPolynomial2));
            List<Term> differentiatedSecondPolynomial = new List<Term>();
            differentiatedSecondPolynomial.AddRange(Term.PowerRuleDifferentiate(secondPolynomial2));
            //multiply first derivative by second polynomial(standard):
            List<Term> firstDerivativeBySecondPolynomial = new List<Term>(Term.PolynomialMultiplication(differentiatedFirstPolynomial, secondPolynomialRaw));
            //multiply second derivative by first polynomial(standard):
            List<Term> secondDerivativeByFirstPolynomial = new List<Term>(Term.PolynomialMultiplication(differentiatedSecondPolynomial, firstPolynomialRaw));
            //combine the two:
            List<Term> finalDerivativeNotCombined = new List<Term>();
            finalDerivativeNotCombined.AddRange(firstDerivativeBySecondPolynomial);
            finalDerivativeNotCombined.AddRange(secondDerivativeByFirstPolynomial);
            List<Term> answerList = new List<Term>(Term.CombineLikeTerms(finalDerivativeNotCombined));
            //return the answer:
            return answerList;
        }
        public static string ProductRuleDifferentiationStringReturn(String expression)
        {
            expression = expression.Replace('*', ',');
            expression = expression.Replace(")(", ",");
            expression = expression.Trim('(');
            expression = expression.Trim(')');
            string[] expressionArray = expression.Split(',');
            List<Term> firstPolynomialRaw = new List<Term>(Term.CreateTermsFromStringToList(expressionArray[0]));
            List<Term> secondPolynomialRaw = new List<Term>(Term.CreateTermsFromStringToList(expressionArray[1]));
            List<Term> firstPolynomial = new List<Term>();
            List<Term> secondPolynomial = new List<Term>();
            firstPolynomial.AddRange(firstPolynomialRaw);
            secondPolynomial.AddRange(secondPolynomialRaw);
            List<Term> firstPolynomial2 = new List<Term>();
            List<Term> secondPolynomial2 = new List<Term>();
            firstPolynomial2.AddRange(firstPolynomialRaw);
            secondPolynomial2.AddRange(secondPolynomialRaw);
            //now differentiate the two equations:
            List<Term> differentiatedFirstPolynomial = new List<Term>();
            differentiatedFirstPolynomial.AddRange(Term.PowerRuleDifferentiate(firstPolynomial2));
            List<Term> differentiatedSecondPolynomial = new List<Term>();
            differentiatedSecondPolynomial.AddRange(Term.PowerRuleDifferentiate(secondPolynomial2));
            //multiply first derivative by second polynomial(standard):
            List<Term> firstDerivativeBySecondPolynomial = new List<Term>(Term.PolynomialMultiplication(differentiatedFirstPolynomial, secondPolynomialRaw));
            //multiply second derivative by first polynomial(standard):
            List<Term> secondDerivativeByFirstPolynomial = new List<Term>(Term.PolynomialMultiplication(differentiatedSecondPolynomial, firstPolynomialRaw));
            //combine the two:
            List<Term> finalDerivativeNotCombined = new List<Term>();
            finalDerivativeNotCombined.AddRange(firstDerivativeBySecondPolynomial);
            finalDerivativeNotCombined.AddRange(secondDerivativeByFirstPolynomial);
            List<Term> answerList = new List<Term>(Term.CombineLikeTerms(finalDerivativeNotCombined));
            //return the answer:
            string answer = "";
            foreach (Term term in answerList)
            {
                if (term.power == 0)
                {
                    answer = answer + term.coefficient;
                }
                else
                {
                    answer = answer + term.coefficient + "x^" + term.power + "  ";
                }
            }
            return answer;
        }
        public static string QoutientRuleDifferentiation(String quotientPolynomial)
        {
            quotientPolynomial = quotientPolynomial.Replace(")/(", ",");
            quotientPolynomial = quotientPolynomial.Replace('/', ',');
            quotientPolynomial = quotientPolynomial.Trim('(');
            quotientPolynomial = quotientPolynomial.Trim(')');
            quotientPolynomial = quotientPolynomial.Trim(',');
            //store in a list and then shield:
            String[] quotientPolynomialArray = quotientPolynomial.Split(',');
            List<Term> numeratorPolynomialRaw = new List<Term>(Term.CreateTermsFromStringToList(quotientPolynomialArray[0]));
            List<Term> denominatorPolynomialRaw = new List<Term>(Term.CreateTermsFromStringToList(quotientPolynomialArray[1]));
            List<Term> numeratorPolynomial = new List<Term>();
            numeratorPolynomial.AddRange(numeratorPolynomialRaw);
            List<Term> denominatorPolynomial = new List<Term>();
            denominatorPolynomial.AddRange(denominatorPolynomialRaw);
            List<Term> numeratorPolynomial2 = new List<Term>();
            numeratorPolynomial2.AddRange(numeratorPolynomialRaw);
            List<Term> denominatorPolynomial2 = new List<Term>();
            denominatorPolynomial2.AddRange(denominatorPolynomialRaw);
            //calculate derivatives for the two equations:
            List<Term> differentiatedNumerator = new List<Term>();
            differentiatedNumerator.AddRange(Term.PowerRuleDifferentiate(numeratorPolynomial2));
            List<Term> differentiatedDenominator = new List<Term>();
            differentiatedDenominator.AddRange(Term.PowerRuleDifferentiate(denominatorPolynomial2));
            //multiply differentiated denominator by regular numerator:
            List<Term> differentiatedDenominatorByNumerator = new List<Term>(Term.PolynomialMultiplication(differentiatedDenominator, numeratorPolynomialRaw));//den'(num)
            //multiply differentiated numerator by regular denominator:
            List<Term> differentiatedNumeratorByDenominator = new List<Term>(Term.PolynomialMultiplication(differentiatedNumerator, denominatorPolynomialRaw));//num'(den)
            //subtract: den'(num) - num'(den)
            //first make num'(den) negative:
            List<Term> negativeDifferentiatedNumeratorByDenominator = new List<Term>(Term.MakePolynomialNegative(differentiatedNumeratorByDenominator));
            //now combine den'(num) and negativeDifferentiatedNumeratorByDenominator:
            List<Term> finalDerivativeNumeratorNotCombined = new List<Term>();
            finalDerivativeNumeratorNotCombined.AddRange(differentiatedDenominatorByNumerator);
            finalDerivativeNumeratorNotCombined.AddRange(negativeDifferentiatedNumeratorByDenominator);
            List<Term> finalNumerator = new List<Term>(Term.CombineLikeTerms(finalDerivativeNumeratorNotCombined));
            //square the denominator:
            List<Term> denominatorSquaredNotCombined = new List<Term>(Term.PolynomialMultiplication(denominatorPolynomialRaw, denominatorPolynomialRaw));
            List<Term> denominatorSquared = new List<Term>(Term.CombineLikeTerms(denominatorSquaredNotCombined));
            //display the answer:
            string answer = "( ";
            foreach (Term term in finalNumerator)
            {
                if (term.power == 0)
                {
                    answer = answer + term.coefficient + " ";
                }
                else
                {
                    answer = answer + term.coefficient + "x^" + term.power + " ";
                }
            }
            answer = answer + ") / ( ";
            foreach (Term term in denominatorSquared)
            {
                if (term.power == 0)
                {
                    answer = answer + term.coefficient + " ";
                }
                else
                {
                    answer = answer + term.coefficient + "x^" + term.power + " ";
                }
            }
            answer = answer + ")";
            return answer;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            for ( ; ; )
            {
                Console.WriteLine("Please enter an equation for differentiation:");
                string expression = Console.ReadLine();
                if (expression.Length == 0)
                {
                    break;
                }
                string answer = "";
                if (expression.Contains("/"))
                {
                    answer = answer + Term.QoutientRuleDifferentiation(expression);
                }
                else
                {
                    if (expression.Contains("*"))
                    {
                        answer = answer + Term.ProductRuleDifferentiationStringReturn(expression);
                    }
                    else
                    {
                        if (expression.Contains(")("))
                        {
                            answer = answer + Term.ProductRuleDifferentiationStringReturn(expression);
                        }
                        else
                        {
                            answer = answer + Term.PowerRuleDifferentiateStringReturn(expression);
                        }
                    }
                }
                Console.WriteLine(answer);
                Console.ReadLine();
            }
        }
    }
}
