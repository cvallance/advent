use day_06::shak::process;
use miette::Context;
use std::thread::sleep;
use std::time::Duration;

#[cfg(feature = "dhat-heap")]
#[global_allocator]
static ALLOC: dhat::Alloc = dhat::Alloc;

#[tracing::instrument]
fn main() -> miette::Result<()> {
    #[cfg(feature = "dhat-heap")]
    let _profiler = dhat::Profiler::new_heap();

    #[cfg(not(feature = "dhat-heap"))]
    tracing_subscriber::fmt::init();

    sleep(Duration::from_micros(190));
    let file = include_str!("../../input.txt");
    let result = process(file).context("process shak")?;
    println!("{}", result);
    Ok(())
}
