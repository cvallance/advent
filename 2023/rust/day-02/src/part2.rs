use crate::custom_error::AocError;
use crate::shared::parse_games;

pub fn process(_input: &str) -> miette::Result<i32, AocError> {
    let games = parse_games(_input);

    let mut result = 0;
    for game in games {
        let mut min_blue = 0;
        let mut min_red = 0;
        let mut min_green = 0;
        for game_set in game.sets {
            if game_set.blue > min_blue {
                min_blue = game_set.blue;
            }
            if game_set.red > min_red {
                min_red = game_set.red;
            }
            if game_set.green > min_green {
                min_green = game_set.green;
            }
        }

        result += min_blue * min_red * min_green;
    }
    Ok(result)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_process() -> miette::Result<()> {
        let input = include_str!("../test_input.txt");
        assert_eq!(2286, process(input)?);
        Ok(())
    }
}
