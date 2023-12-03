pub struct Game {
    pub id: i32,
    pub sets: Vec<GameSet>,
}

pub struct GameSet {
    pub blue: i32,
    pub red: i32,
    pub green: i32,
}

pub fn parse_games(input: &str) -> Vec<Game> {
    let mut games = Vec::new();

    for line in input.lines() {
        let parts: Vec<&str> = line.split(':').collect();
        if parts.len() != 2 {
            continue;
        }

        let id = parts[0]
            .split_whitespace()
            .nth(1)
            .unwrap()
            .parse::<i32>()
            .unwrap();
        let sets_str: Vec<&str> = parts[1].trim().split(';').collect();
        let mut sets = Vec::new();
        for set_str in sets_str {
            sets.push(parse_game_set(set_str));
        }

        games.push(Game { id, sets });
    }

    games
}

fn parse_game_set(set_str: &str) -> GameSet {
    let mut blue = 0;
    let mut red = 0;
    let mut green = 0;

    let parts: Vec<&str> = set_str.split(',').collect();
    for part in parts {
        let [count, color]: [&str; 2] = part
            .split_whitespace()
            .collect::<Vec<&str>>()
            .try_into()
            .unwrap();
        match color {
            "blue" => blue = count.parse().unwrap(),
            "red" => red = count.parse().unwrap(),
            "green" => green = count.parse().unwrap(),
            _ => (),
        }
    }

    GameSet { blue, red, green }
}
