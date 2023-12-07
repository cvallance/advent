use std::cmp::Ordering;
use std::collections::HashMap;
use strum::IntoEnumIterator;
use strum_macros::EnumIter;

#[derive(Debug)]
pub enum HandType {
    HighCard,
    OnePair,
    TwoPairs,
    ThreeOfAKind,
    FullHouse,
    FourOfAKind,
    FiveOfAKind,
}

#[derive(Debug, Hash, PartialEq, Eq, Clone, Copy, EnumIter, Ord, PartialOrd)]
pub enum Card {
    Joker,
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Queen,
    King,
    Ace,
}

impl From<char> for Card {
    fn from(c: char) -> Self {
        match c {
            '1' => Card::One,
            '2' => Card::Two,
            '3' => Card::Three,
            '4' => Card::Four,
            '5' => Card::Five,
            '6' => Card::Six,
            '7' => Card::Seven,
            '8' => Card::Eight,
            '9' => Card::Nine,
            'T' => Card::Ten,
            'J' => Card::Joker,
            'Q' => Card::Queen,
            'K' => Card::King,
            'A' => Card::Ace,
            _ => panic!("Invalid card"),
        }
    }
}

#[derive(Debug)]
pub struct HandAndBet {
    pub cards: Vec<Card>,
    pub card_counts: HashMap<Card, u64>,
    pub bet: u64,
}

impl HandAndBet {
    fn new(line: &str) -> Self {
        let (cards, bet) = line.split_once(" ").unwrap();
        let cards = cards.chars().map(|c| c.into()).collect::<Vec<_>>();

        let mut card_counts = new_card_counts();
        for card in &cards {
            let count = card_counts.entry(*card).or_insert(0);
            *count += 1;
        }
        HandAndBet {
            cards,
            card_counts,
            bet: bet.parse().unwrap(),
        }
    }

    pub fn hand_type(&self) -> HandType {
        let mut without_jokers = HashMap::new();
        for (card, count) in self.card_counts.iter() {
            if card == &Card::Joker {
                continue;
            }
            without_jokers.insert(card.clone(), count.clone());
        }
        let joker_count = self.card_counts.get(&Card::Joker).unwrap_or(&0);

        if without_jokers.iter().any(|(_, count)| *count == 5) {
            return HandType::FiveOfAKind;
        } else if without_jokers.iter().any(|(_, count)| *count == 4) {
            if *joker_count == 1 {
                return HandType::FiveOfAKind;
            } else {
                return HandType::FourOfAKind;
            }
        } else if without_jokers.iter().any(|(_, count)| *count == 3) {
            if without_jokers.iter().any(|(_, count)| *count == 2) {
                return HandType::FullHouse;
            } else {
                if *joker_count == 2 {
                    return HandType::FiveOfAKind;
                } else if *joker_count == 1 {
                    return HandType::FourOfAKind;
                } else {
                    return HandType::ThreeOfAKind;
                }
            }
        } else if without_jokers
            .iter()
            .filter(|(_, count)| **count == 2)
            .count()
            == 2
        {
            if *joker_count == 1 {
                return HandType::FullHouse;
            }
            return HandType::TwoPairs;
        } else if without_jokers.iter().any(|(_, count)| *count == 2) {
            if *joker_count == 3 {
                return HandType::FiveOfAKind;
            } else if *joker_count == 2 {
                return HandType::FourOfAKind;
            } else if *joker_count == 1 {
                return HandType::ThreeOfAKind;
            }
            return HandType::OnePair;
        }

        if *joker_count == 5 {
            return HandType::FiveOfAKind;
        } else if *joker_count == 4 {
            return HandType::FiveOfAKind;
        } else if *joker_count == 3 {
            return HandType::FourOfAKind;
        } else if *joker_count == 2 {
            return HandType::ThreeOfAKind;
        } else if *joker_count == 1 {
            return HandType::OnePair;
        }

        HandType::HighCard
    }

    fn hand_rank(&self) -> u64 {
        match self.hand_type() {
            HandType::HighCard => 1,
            HandType::OnePair => 2,
            HandType::TwoPairs => 3,
            HandType::ThreeOfAKind => 4,
            HandType::FullHouse => 5,
            HandType::FourOfAKind => 6,
            HandType::FiveOfAKind => 7,
        }
    }
}

impl PartialEq<Self> for HandAndBet {
    fn eq(&self, other: &Self) -> bool {
        // I'm assuming that there are no duplicate hands
        false
    }
}

impl Eq for HandAndBet {}

impl PartialOrd for HandAndBet {
    fn partial_cmp(&self, other: &Self) -> Option<Ordering> {
        let self_rank = self.hand_rank();
        let other_rank = other.hand_rank();
        let rank_cmp = self_rank.partial_cmp(&other_rank);
        if rank_cmp != Some(Ordering::Equal) {
            return rank_cmp;
        }

        for (self_card, other_card) in self.cards.iter().zip(other.cards.iter()) {
            let card_cmp = self_card.partial_cmp(other_card);
            if card_cmp != Some(Ordering::Equal) {
                return card_cmp;
            }
        }

        None
    }
}

impl Ord for HandAndBet {
    fn cmp(&self, other: &Self) -> Ordering {
        let self_rank = self.hand_rank();
        let other_rank = other.hand_rank();
        let rank_cmp = self_rank.cmp(&other_rank);
        if rank_cmp != Ordering::Equal {
            return rank_cmp;
        }

        for (self_card, other_card) in self.cards.iter().zip(other.cards.iter()) {
            let card_cmp = self_card.cmp(other_card);
            if card_cmp != Ordering::Equal {
                return card_cmp;
            }
        }

        Ordering::Equal
    }
}

fn new_card_counts() -> HashMap<Card, u64> {
    let mut card_counts = HashMap::new();
    for card in Card::iter() {
        card_counts.insert(card, 0);
    }
    card_counts
}

pub fn parse_input(input: &str) -> Vec<HandAndBet> {
    input.lines().map(|line| HandAndBet::new(line)).collect()
}
