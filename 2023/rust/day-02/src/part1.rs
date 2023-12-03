use crate::custom_error::AocError;
use crate::shared::parse_games;

pub fn process(
    _input: &str,
) -> miette::Result<i32, AocError> {
    let max_blue = 14;
    let max_red = 12;
    let max_green = 13;
    let games = parse_games(_input);

    let mut result = 0;
    'outer: for game in games {
        for game_set in game.sets {
            if game_set.blue > max_blue || game_set.red > max_red || game_set.green > max_green {
                continue 'outer;
            }
        }
        result += game.id;
    }
    Ok(result)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_process() -> miette::Result<()> {
        let input = include_str!("../test_input.txt");
        assert_eq!(8, process(input)?);
        Ok(())
    }
}
