import type {ReactNode} from 'react';
import clsx from 'clsx';
import Link from '@docusaurus/Link';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import Layout from '@theme/Layout';
import HomepageFeatures from '@site/src/components/HomepageFeatures';
import Heading from '@theme/Heading';
import CodeBlock from '@theme/CodeBlock';

import styles from './index.module.css';

function HomepageHeader() {
  const {siteConfig} = useDocusaurusContext();
  return (
    <header className={clsx('hero', styles.heroBanner)}>
      <div className="container">
        <Heading as="h1" className="hero__title">
          {siteConfig.title}
        </Heading>
        <p className="hero__subtitle">{siteConfig.tagline}</p>
        <div className={styles.buttons}>
          <Link className="button button--secondary button--lg" to="/getting-started">
            Get started
          </Link>
        </div>
      </div>
    </header>
  );
}

const example = `DockerComposeFile file = new DockerComposeBuilder()
    .WithService("web", service => service
        .WithImage("nginx:1.27")
        .WithPort(8080, 80)
        .WithNetwork("front"))
    .WithNetwork("front", network => network.WithDriver("bridge"))
    .Build();

string yaml = file.ToYaml();`;

export default function Home(): ReactNode {
  const {siteConfig} = useDocusaurusContext();
  return (
    <Layout
      title={siteConfig.title}
      description="A fluent .NET API for generating docker-compose.yml files.">
      <HomepageHeader />
      <main>
        <HomepageFeatures />
        <section className={styles.example}>
          <div className="container">
            <Heading as="h2">Build a compose file, get back YAML</Heading>
            <CodeBlock language="csharp">{example}</CodeBlock>
          </div>
        </section>
      </main>
    </Layout>
  );
}
